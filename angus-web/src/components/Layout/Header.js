import React, {Component} from 'react';
import pubsub from 'pubsub-js';
import {Link} from 'react-router-dom';
import {UncontrolledDropdown, DropdownToggle, DropdownMenu, DropdownItem, ListGroup, ListGroupItem} from 'reactstrap';

import ToggleState from '../Common/ToggleState';
import TriggerResize from '../Common/TriggerResize';
import ToggleFullscreen from '../Common/ToggleFullscreen';
import HeaderRun from './Header.run'

import {deleteLocations} from '../../Services/AngusAPI'

class Header extends Component {

  componentDidMount() {

    HeaderRun();

  }

  toggleUserblock(e) {
    e.preventDefault();
    pubsub.publish('toggleUserblock');
  }

  deleteLocations(e) {
    e.preventDefault();
    deleteLocations()
      .then(() => {
      })
      .catch((error) => {
        console.log(error)
      });
  }

  render() {
    return (
      <header className="topnavbar-wrapper">
        {/* START Top Navbar */}
        <nav className="navbar topnavbar">
          {/* START navbar header */}
          <div className="navbar-header">
            <a className="navbar-brand" href="#/">
              <div className="brand-logo">
                <img className="img-fluid" src="img/logo.svg" alt="Angus"/>
                <span>Angus</span>
              </div>
              <div className="brand-logo-collapsed">
                <img className="img-fluid" src="img/logo.svg" alt="Angus"/>
              </div>
            </a>
          </div>
          {/* END navbar header */}

          {/* START Left navbar */}
          <ul className="navbar-nav mr-auto flex-row">
            <li className="nav-item">
              {/* Button used to collapse the left sidebar. Only visible on tablet and desktops */}
              <TriggerResize>
                <ToggleState state="aside-collapsed">
                  <a href="" className="nav-link d-none d-md-block d-lg-block d-xl-block">
                    <em className="fa fa-navicon"></em>
                  </a>
                </ToggleState>
              </TriggerResize>
              {/* Button to show/hide the sidebar on mobile. Visible on mobile only. */}
              <ToggleState state="aside-toggled" nopersist={true}>
                <a href="" className="nav-link sidebar-toggle d-md-none">
                  <em className="fa fa-navicon"></em>
                </a>
              </ToggleState>
            </li>
          </ul>
          {/* END Left navbar */}
          {/* START Right Navbar */}
          <ul className="navbar-nav flex-row">
            {/* START Alert menu */}
            <UncontrolledDropdown nav inNavbar className="dropdown-list">
              <DropdownToggle nav className="dropdown-toggle-nocaret mr-2">
                <em className="icon-bell"></em>
                <span className="badge badge-danger">7</span>
              </DropdownToggle>
              {/* START Dropdown menu */}
              <DropdownMenu right className="dropdown-menu-right animated flipInX">
                <DropdownItem>
                  {/* START list group */}
                  <ListGroup>
                    <ListGroupItem action tag="a" href="" onClick={e => e.preventDefault()}>
                      <div className="media">
                        <div className="align-self-start mr-2">
                          <em className="fa fa-location-arrow fa-2x text-info"></em>
                        </div>
                        <div className="media-body">
                          <p className="m-0">Position</p>
                          <p className="m-0 text-muted text-sm">Margueritte est isolée</p>
                        </div>
                      </div>
                    </ListGroupItem>
                    <ListGroupItem action tag="a" href="" onClick={e => e.preventDefault()}>
                      <div className="media">
                        <div className="align-self-start mr-2">
                          <em className="fa fa-thermometer fa-2x text-warning"></em>
                        </div>
                        <div className="media-body">
                          <p className="m-0">Température</p>
                          <p className="m-0 text-muted text-sm">Température élevée pour Zelda</p>
                        </div>
                      </div>
                    </ListGroupItem>
                    <ListGroupItem action tag="a" href="" onClick={e => e.preventDefault()}>
                                          <span className="d-flex align-items-center">
                                             <span className="text-sm">Plus de notifications</span>
                                             <span className="badge badge-danger ml-auto">5</span>
                                          </span>
                    </ListGroupItem>
                  </ListGroup>
                  {/* END list group */}
                </DropdownItem>
              </DropdownMenu>
              {/* END Dropdown menu */}
            </UncontrolledDropdown>
            {/* END Alert menu */}
          </ul>
          {/* END Right Navbar */}

          {/* START Search form */}
          <form className="navbar-form" role="search" action="search.html">
            <div className="form-group">
              <input className="form-control" type="text" placeholder="Type and hit enter ..."/>
              <div className="fa fa-times navbar-form-close" data-search-dismiss=""></div>
            </div>
            <button className="d-none" type="submit">Submit</button>
          </form>
          {/* END Search form */}
        </nav>
        {/* END Top Navbar */}
      </header>
    );
  }

}

export default Header;
