Ext.define('HomeMPC.controller.Mpc', {
  extend: 'Ext.app.Controller',
  stores: ['Files', 'PlayList' ],
  models: ['File', 'PlayListFile'],
  
  init: function () {
    this.control({
      'viewport > panel' : {
        render: this.onPanelRendered
      }
    });
  },
  
  onPanelRendered: function(){    
  }
});