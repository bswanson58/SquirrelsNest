import {Component} from '@angular/core'
import {NgForm} from '@angular/forms'
import {AuthService} from '../auth.service'

@Component( {
  selector: 'sn-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
} )
export class LoginComponent {

  constructor( private loginService: AuthService ) {
  }

  onSubmit( form: NgForm ) {
    this.loginService.Login( form.value.email, form.value.password )
  }
}
