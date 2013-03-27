Ext.define('HomeMPC.view.FileTree', {
  extend: 'Ext.tree.Panel',
  title: 'Files',
  alias: 'widget.filetree',
  rootVisible: false,
  useArrows: true,  
  store: 'Files'
});