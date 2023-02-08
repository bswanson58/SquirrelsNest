import {Component, Inject, ViewEncapsulation} from '@angular/core'
import {MAT_SNACK_BAR_DATA} from '@angular/material/snack-bar'

@Component( {
  selector: 'sn-error-panel',
  templateUrl: './error-panel.component.html',
  styleUrls: ['./error-panel.component.css'],
  encapsulation: ViewEncapsulation.None
} )
export class ErrorPanelComponent {
  message: string = ''

  constructor( @Inject( MAT_SNACK_BAR_DATA ) message: string ) {
    this.message = message
  }
}
