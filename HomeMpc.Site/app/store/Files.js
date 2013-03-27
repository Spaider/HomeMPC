Ext.define('HomeMPC.store.Files', {
  extend: 'Ext.data.TreeStore',
  requires: 'HomeMPC.model.File',
  proxy: {
    type: 'ajax',
    url: 'Home/Files',
    reader: {
      type: 'json',
    }
  },
  // model: 'HomeMPC.model.File',
  root: {
    id: '',
    text: '.',
    expanded: true
  },
  nodeParam: 'path'
});