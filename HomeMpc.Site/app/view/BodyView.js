Ext.define('HomeMPC.view.BodyView', {
  extend: 'Ext.panel.Panel',
  alias: 'widget.bodyview',
  initComponent: function () {
    this.items = [
      {
        xtype: 'button',
        text: 'Play list',
        listeners: {
          click: this.onTestButtonClick
        }
      }
    ];
    this.callParent();
  },
  
  onTestButtonClick : function(){
    alert('Click!');
  }
});