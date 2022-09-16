import {Component, Inject, ViewEncapsulation} from '@angular/core'
import {MAT_SNACK_BAR_DATA} from '@angular/material/snack-bar'

@Component({
  selector: 'sn-service-activity-panel',
  templateUrl: './service-activity-panel.component.html',
  styleUrls: ['./service-activity-panel.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class ServiceActivityPanelComponent {
  message: string = ''

  constructor( @Inject( MAT_SNACK_BAR_DATA ) message: string ) {
    this.message = message
  }
}
