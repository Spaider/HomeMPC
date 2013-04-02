Ext.define('HomeMPC.model.PlayListFile', {
  extend: 'Ext.data.Model',
  fields: [
    { name: 'Id' }, 
    { name: 'Title'}, 
    { name: 'Album' }, 
    { name: 'Artist' }, 
    { name: 'Track', type: 'int' }],
  proxy: {
    type: 'ajax',
    url: 'Mpc/PlayList',
    reader: {
      type: 'json'
    }
  }
});