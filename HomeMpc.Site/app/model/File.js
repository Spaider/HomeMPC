Ext.define('HomeMPC.model.File', {
  extend: 'Ext.data.Model',
  fields: [
    { name: 'fullpath', type: 'string' },
    { name: 'text', type: 'string' }
  ],
  proxy: {
    type: 'ajax',
    url: 'Mpc/Files',
    reader: {
      type: 'json',
    }
  }
});