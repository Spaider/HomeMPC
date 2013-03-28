Ext.define('HomeMPC.store.PlayList', {
  extend: 'Ext.data.Store',
  requires: 'HomeMPC.model.PlayListFile',
  model: 'HomeMPC.model.PlayListFile', // Must be fully namespaced name!  
  autoLoad: true
});