Ext.define('HomeMPC.view.ControlPanel', {
  extend: 'Ext.panel.Panel',
  alias: 'widget.controlpanel',
  layout: 'hbox',
  bodyCls: 'x-btn-group-default-framed-mc',
  items: [{
    xtype: 'panel',
    width: 52,
    height: 48,
    html: '<img src="/resources/images/logo.png" width="48" height="48" border="0" alt="MPC Home" title="Home MPC" />',
    border: 0,
    bodyPadding: "0 2 0 2",
    bodyCls: 'x-btn-group-default-framed-mc',
    },{
      xtype: 'buttongroup',
      bodyPadding: 2,
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