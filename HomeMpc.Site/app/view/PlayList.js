Ext.define('HomeMPC.view.PlayList', {
  extend: 'Ext.grid.Panel',
  alias: 'widget.playlist',
  columns: [
    { header: '#', dataIndex: 'Id', width: 30, align: 'right' },
    { header: 'Track', dataIndex: 'Track', width: 50, align: 'right'},
    { header: 'Title', dataIndex: 'Title', width: 300 },
    { header: 'Artist', dataIndex: 'Artist', width: 300 },
    { header: 'Album', dataIndex: 'Album', width: 300 }
  ],
  store: 'PlayList',
  tbar: [{
    xtype: 'buttongroup',
    items: [{
      text: 'Clear',
      action: 'clear',
      tooltip: 'Clear playlist',
      icon: '/Resources/Images/playlist-clear.png'
    }]
  }]
});