const Menu = [
  {
    heading: 'Main Navigation',
    translate: 'sidebar.heading.HEADER'
  },
  {
    name: 'Carte',
    path: 'home',
    icon: 'icon-map',
    translate: 'sidebar.nav.SINGLEVIEW'
  },
  {
    name: 'Alertes',
    icon: 'icon-speedometer',
    translate: 'sidebar.nav.MENU',
    label: {value: 1, color: 'info'},
    submenu: [{
      name: 'Isolement',
      translate: 'sidebar.nav.SUBMENU',
      path: 'submenu'
    },
      {
        name: 'Températures',
        translate: 'sidebar.nav.SUBMENU',
        path: 'submenu'
      }
    ]
  },
  {
    name: 'Paturages',
    path: 'grazing',
    icon: 'icon-grid',
  }
];

export default Menu;
