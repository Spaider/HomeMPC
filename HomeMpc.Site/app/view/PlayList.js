Ext.define('HomeMPC.view.PlayList', {
  extend: 'Ext.grid.Panel',
  alias: 'widget.playlist',
  columns: [
    { header: 'Title', dataIndex: 'Title', width: 300 },
    { header: 'Album', dataIndex: 'Album', width: 300 }
  ],
  store: 'PlayList'
});