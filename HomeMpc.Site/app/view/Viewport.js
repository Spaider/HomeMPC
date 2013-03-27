Ext.define('HomeMPC.view.Viewport', {
  extend: 'Ext.container.Viewport',
  layout: 'fit',
  requires: [
    'HomeMPC.view.FileTree'
  ],
  initComponent: function() {
    this.items = {
      xtype : 'panel',
      layout: 'border',
      items: [{
          title: 'Center',
          region: 'center',
          html: 'Center',
          xtype: 'panel',
          layout: 'fit'
        }, {
          title: 'Files',
          region: 'west',
          xtype: 'filetree',
          width: 400,
          split: true,
          collapsible: true
        }]
    };
    this.callParent();
  }
});