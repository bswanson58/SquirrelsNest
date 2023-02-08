import {Component} from '@angular/core'
import {BehaviorSubject} from 'rxjs'
import {AddUserInput} from '../../Data/graphQlTypes'
import {UsersFacade} from '../../Users/user.facade'

@Component( {
  selector: 'sn-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
} )
export class RegisterComponent {
  name: string
  email: string
  password: string
  retypedPassword: string
  registrationSucceeded$: BehaviorSubject<boolean>
  registrationFailed$: BehaviorSubject<boolean>
  registrationMessage: string

  constructor( private userFacade: UsersFacade ) {
    this.name = ''
    this.email = ''
    this.password = ''
    this.retypedPassword = ''

    this.registrationSucceeded$ = new BehaviorSubject<boolean>( false )
    this.registrationFailed$ = new BehaviorSubject<boolean>( false )
    this.registrationMessage = ''
  }

  onRegister() {
    const userInput: AddUserInput = {
      name: this.name,
      loginName: this.email,
      email: this.email,
      password: this.password
    }

    this.userFacade.AddUserWithCallback( userInput, ( success: boolean, message: string ) => this.registerSucceeded( success, message ) )
  }

  private registerSucceeded( success: boolean, message: string ) {
    this.registrationMessage = message ?? ''

    if( success ) {
      this.registrationSucceeded$.next( true )
    }
    else {
      this.registrationFailed$.next( true )
    }
  }
}
