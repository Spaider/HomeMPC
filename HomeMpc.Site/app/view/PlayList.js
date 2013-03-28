Ext.define('HomeMPC.view.PlayList', {
  extend: 'Ext.grid.Panel',
  alias: 'widget.playlist',
  columns: [
    { header: '#', dataIndex: 'Id', width: 30, align: 'right' },
    { header: 'Title', dataIndex: 'Title', width: 300 },
    { header: 'Artist', dataIndex: 'Artist', width: 300 },
    { header: 'Album', dataIndex: 'Album', width: 300 }
  ],
  store: 'PlayList'
});