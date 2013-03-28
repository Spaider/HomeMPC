Ext.define('HomeMPC.model.PlayListFile', {
  extend: 'Ext.data.Model',
  fields: ['Title', 'Album'],
  proxy: {
    type: 'ajax',
    url: 'Mpc/PlayList',
    reader: {
      type: 'json'
    }
  }
});