var React = require('react');
var ReactDOM = require('react-dom');
var Rating = require('react-rating');
var pubsub = require('pubsub-js');


var ChatBox = React.createClass({
  getInitialState: function () {
    return {
      answer: '',
      displayBox: [],
    };
  },

  componentWillMount: function () {
    // when React renders me, I subscribe to the topic 'products'
    // .subscribe returns a unique token necessary to unsubscribe
    var ansStyleOptions = { clear: 'both', backgroundColor: '#ffe6cb', marginBottom: '15px' };
    var quesStyleOption = { backgroundColor: '#c7eafc', marginBottom: '15px' };
    this.pubsub_tokenQ = pubsub.subscribe('questions', (topic, quest) => {
      let view = <div className="questionView"><span className="pull-left" style={quesStyleOption }>{quest}</span></div>
      this.setState({ displayBox: this.state.displayBox.concat([view]) });
      console.log(quest);
    });
    this.pubsub_tokenA = pubsub.subscribe('answers', (topic, answer) => {
      let view = <div className="answerView"><span className="pull-right" style={ansStyleOptions }>{answer.ans}</span></div>
      this.setState({ displayBox: this.state.displayBox.concat([view]) });
      console.log(answer.ans);
    });
  },

  componentDidUpdate: function () {
    var node = this.refs.scroll;
    node.scrollTop = node.scrollHeight;
  },

  componentWillUnmount: function () {
    // React removed me from the DOM, I have to unsubscribe from the pubsub using my token
    pubsub.unsubscribe(this.pubsub_tokenQ);
    pubsub.unsubscribe(this.pubsub_tokenA);
  },


  render: function () {
    var scrollOptions = {
      overflowY: 'auto',
      height: '300px',
      width: 'auto'
    };
    return ( <div className="row">
     <div className="col-md-3"></div>
      <div className="col-md-6 form-group">
        <div className="displayArea" style={scrollOptions} ref='scroll'>
          {this.state.displayBox}
        </div>
        <hr />
        <QuestionBox />
      </div>
      <div className="col-md-3"></div>
    </div>
    );
  }
});


var QuestionBox = React.createClass({


  getInitialState: function () {
    return {
      SurveyId: '',
      QuestionID: '',
      question: '',
      answerType: '',
    };
  },

  _getQuestion: function (url) {
    $.getJSON(url, function (result) {
      this.setState({
        SurveyId: result["SurveyId"], QuestionID: result["QuestionID"],
        question: result["question"], answerType: result["AnswerType"]
      });
      pubsub.publish('questions', this.state.question);
    }.bind(this));
  },

  componentDidMount: function () {
    const url = `http://localhost:56237/api/Survey/${reqSurveyId}/Question/${reqQuestionId}`  ;
    this._getQuestion(url);
  },

  componentWillUnmount: function () {
    this._getQuestion.abort();
  },



  _handleResponse: function (questionFromServer) {
    pubsub.publish('questions', questionFromServer);
    this.setState({ question: questionFromServer });
  },

  render: function () {
    return (<div className="questionBox">
      <AnswerBox callbackParent={this._handleResponse} surveyId={this.state.SurveyId}
                 questionID={this.state.QuestionID} answerType={this.state.answerType} />
    </div>);
  }
});

var AnswerBox = React.createClass({

  getInitialState: function () {
    return {
      SurveyId: this.props.surveyId,
      QuestionID: this.props.questionID,
      question: '',
      answerType: this.props.answerType,
      value: '',
      Answer: ''
    };
  },

  postData: function (e) {
    let answerUrl = "http://localhost:56237/api/Answers";
    var json = {};

    json["SurveyId"] = (this.state.SurveyId == "") ? this.props.surveyId : this.state.SurveyId;
    json["QuestionId"] = (this.state.QuestionID == "") ? this.props.questionID : this.state.QuestionID;
    json["AnswerType"] = (this.state.answerType == "") ? this.props.answerType : this.state.answerType;
    if (json["AnswerType"] !== 1) {
      json["Answer"] = this._textarea.value;//$('#textarea').val();
    }
    else {
      json["Answer"] = e;
    }
    $.ajax({
      url: answerUrl,
      type: "POST",
      data: json,
      dataType: "json",
    }).done((result) => {
      if (result !== "") {
        this.setState({ SurveyId: result.SurveyId, QuestionID: result.QuestionID, answerType: result.AnswerType, Answer: json["Answer"] });
        let obj = { ans: this.state.Answer };
        pubsub.publish('answers', obj);
        if (json["AnswerType"] !== 1) {
          this._textarea.value = '';
        }
        this.props.callbackParent(result.question);
      }
      else {
        alert("Thanks for completing the survey. Have a great day");
        document.location.href = homeUrl;
      }
    }).error((j, ex) => {
      console.log(j.responseText);
      this._textarea.value = '';
    })
    e.preventDefault();
  },

  render: function () {
    let view = "";
    let toggle = (this.state.answerType == "") ? this.props.answerType : this.state.answerType;
    if (toggle == 1) {
      view = <div className="ratingArea"><Rating start={0} stop={5} onClick={this.postData } /></div>;
    }
    else {
      view = <div>
        <form className="answerform form-inline" onSubmit={this.postData}>
              <div className="answerBox form-group">
                <textarea className="form-control" ref={(val) => this._textarea = val }></textarea>
              </div>
          &nbsp;&nbsp;
          <button type="submit" className="btn btn-default glyphicon glyphicon-ok"></button>
        </form>
      </div>
    }

    return (<div>{view}</div>);
  }
});

ReactDOM.render(
  <ChatBox />,
  document.getElementById('content')
);
