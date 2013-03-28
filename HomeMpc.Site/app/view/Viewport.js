Ext.define('HomeMPC.view.Viewport', {
  extend: 'Ext.container.Viewport',
  layout: 'fit',
  requires: [
    'HomeMPC.view.FileTree',
    'HomeMPC.view.PlayList'
  ],
  initComponent: function() {
    this.items = {
      xtype : 'panel',
      layout: 'border',
      items: [{
          title: 'Play list',
          region: 'center',
          xtype: 'playlist',
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