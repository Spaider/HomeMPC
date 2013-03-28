Ext.define('HomeMPC.model.PlayListFile', {
  extend: 'Ext.data.Model',
  fields: ['Id', 'Title', 'Album', 'Artist'],
  proxy: {
    type: 'ajax',
    url: 'Mpc/PlayList',
    reader: {
      type: 'json'
    }
  }
});