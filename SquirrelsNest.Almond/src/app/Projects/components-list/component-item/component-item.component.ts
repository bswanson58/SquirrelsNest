import {Component, Input} from '@angular/core'
import {ClComponent} from '../../../Data/graphQlTypes'

@Component( {
  selector: 'sn-component-item',
  templateUrl: './component-item.component.html',
  styleUrls: ['./component-item.component.css']
} )
export class ComponentItemComponent {
  @Input() component!: ClComponent
  isHovering: boolean = false

  constructor() {
  }

  onEdit() {
  }

  onDelete() {
  }
}
