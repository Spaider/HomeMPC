Ext.define('HomeMPC.controller.Mpc', {
  extend: 'Ext.app.Controller',
  stores: ['Files', 'PlayList' ],
  models: ['File', 'PlayListFile'],
  
  init: function () {
    this.control({
      'viewport > panel' : {
        render: this.onPanelRendered},
      'controlpanel button[action=play]': {
        click: this.onPlayClicked
      },
      'controlpanel button[action=pause]': {
        click: this.onPauseClicked
      },
      'controlpanel button[action=stop]': {
        click: this.onStopClicked
      },
      'controlpanel button[action=prev]': {
        click: this.onPrevClicked
      },
      'controlpanel button[action=next]': {
        click: this.onNextClicked
      }
    });
  },
  
  onPanelRendered: function () {
    console.log("Panel rendered");
  },
  
  onPlayClicked: function(){
    console.log('Play');
  },
  
  onStopClicked: function(){
    console.log('Stop');
  },

  onPauseClicked: function () {
    console.log('Pause');
  },

  onPrevClicked: function () {
    console.log('Prev');
  },

  onNextClicked: function () {
    console.log('Next');
  }
  
});