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
      },
      'playlist button[action=clear]': {
        click: this.onPlaylistClearClicked
      }
    });
  },
  
  onPanelRendered: function () {
  },
  
  onPlayClicked: function(){
    console.log('Play');
    Ext.Ajax.request({
      url: '/Mpc/Play',
      success: function (response) {
//        var text = response.responseText;
//        alert(text);
      }
    });
  },
  
  onStopClicked: function(){
    console.log('Stop');
    Ext.Ajax.request({
      url: '/Mpc/Stop',
      success: function (response) {
//        var text = response.responseText;
//        alert(text);
      }
    });
  },

  onPauseClicked: function () {
    console.log('Pause');
    Ext.Ajax.request({
      url: '/Mpc/Pause',
      success: function (response) {
//        var text = response.responseText;
//        alert(text);
      }
    });
  },

  onPrevClicked: function () {
    console.log('Prev');
    Ext.Ajax.request({
      url: '/Mpc/PlayPrevious',
      success: function (response) {
//        var text = response.responseText;
//        alert(text);
      }
    });
  },

  onNextClicked: function () {
    console.log('Next');
    Ext.Ajax.request({
      url: '/Mpc/PlayNext',
      success: function (response) {
//        var text = response.responseText;
//        alert(text);
      }
    });
  },
  
  onPlaylistClearClicked: function() {
    Ext.Msg.show({
      title: 'Home MPC',
      msg: 'Not implemented yet',
      buttons: Ext.Msg.OK,
      icon: Ext.Msg.INFO
    });
  }
});