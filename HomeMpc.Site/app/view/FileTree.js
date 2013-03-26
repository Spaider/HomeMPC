Ext.define('HomeMPC.view.FileTree', {
  extend: 'Ext.tree.Panel',
  title: 'Files',
  alias: 'widget.filetree',
  rootVisible: true,
  useArrows: true,
  root: {
    text: 'Some folder',
    expanded: true,
    children: [
      {
        text: 'Some file',
        leaf: true
      },
      {
        text: 'Another file',
        leaf: true
      },
      {
        text: 'Subfolder',
        expanded: false,
        children: [
          {
            text: 'File1.mp3',
            leaf: true
          },
          {
            text: 'File2.mp3',
            leaf: true
          }
        ]
      }
    ]
  }
});