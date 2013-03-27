Ext.define('HomeMPC.controller.FileTree', {
  extend: 'Ext.app.Controller',
  stores: ['Files'],
  models: ['File'],

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