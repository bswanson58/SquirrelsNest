import {NgModule} from '@angular/core'
import {MatButtonModule} from '@angular/material/button'
import {MatCheckboxModule} from '@angular/material/checkbox'
import {MatRippleModule} from '@angular/material/core'
import {MatDialogModule} from '@angular/material/dialog'
import {MatFormFieldModule} from '@angular/material/form-field'
import {MatIconModule} from '@angular/material/icon'
import {MatInputModule} from '@angular/material/input'
import {MatListModule} from '@angular/material/list'
import {MatSelectModule} from '@angular/material/select'
import {MatSidenavModule} from '@angular/material/sidenav'
import {MatToolbarModule} from '@angular/material/toolbar'

@NgModule( {
  imports: [
    MatButtonModule, MatCheckboxModule, MatDialogModule, MatFormFieldModule,
    MatIconModule, MatInputModule, MatListModule, MatRippleModule, MatSelectModule,
    MatSidenavModule, MatToolbarModule
  ],
  exports: [
    MatButtonModule, MatCheckboxModule, MatDialogModule, MatFormFieldModule,
    MatIconModule, MatInputModule, MatListModule, MatRippleModule, MatSelectModule,
    MatSidenavModule, MatToolbarModule
  ]
} )
export class MaterialModule {
}
