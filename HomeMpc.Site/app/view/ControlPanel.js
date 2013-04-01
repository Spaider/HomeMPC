Ext.define('HomeMPC.view.ControlPanel', {
  extend: 'Ext.panel.Panel',
  alias: 'widget.controlpanel',
  tbar: [{
    xtype: 'buttongroup',
    items: [{
      text: 'Play',
      action: 'play',
      scale: 'large',
      icon: '/resources/images/play-blue.png'
    },{
      text: 'Pause',
      action: 'pause',
      scale: 'large',
      icon: '/resources/images/pause-blue.png'
    },{
      text: 'Stop',
      action: 'stop',
      scale: 'large',
      icon: '/resources/images/stop-blue.png'
    },{
      text: 'Prev',
      action: 'prev',
      scale: 'large',
      icon: '/resources/images/prev-blue.png'
    },{
      text: 'Next',
      action: 'next',
      scale: 'large',
      icon: '/resources/images/next-blue.png'
    }]
  }],
  
  init: function () {
    this.control = ({});
  },
  
  initComponent: function () {
    this.callParent();
  }
});