import {Component} from '@angular/core'
import {Router} from '@angular/router'

@Component( {
  selector: 'sn-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
} )
export class HomePageComponent {

  constructor( private router: Router ) {
  }

  onLogin() {
    this.router.navigate( ['login'] ).then()
  }

  onRegister() {
    this.router.navigate( ['register'] ).then()
  }

}
