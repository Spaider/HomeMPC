Ext.define('HomeMPC.store.Files', {
  extend: 'Ext.data.TreeStore',
  requires: 'HomeMPC.model.File',  
  model: 'HomeMPC.model.File', // Must be fully namespaced name!
  
  root: {
    id: '',
    text: '.',
    expanded: true
  },
  nodeParam: 'path'
});