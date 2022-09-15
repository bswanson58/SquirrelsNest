import {NgModule} from '@angular/core'
import {MatButtonModule} from '@angular/material/button'
import {MatCardModule} from '@angular/material/card'
import {MatCheckboxModule} from '@angular/material/checkbox'
import {MatRippleModule} from '@angular/material/core'
import {MatDialogModule} from '@angular/material/dialog'
import {MatDividerModule} from '@angular/material/divider'
import {MatFormFieldModule} from '@angular/material/form-field'
import {MatIconModule} from '@angular/material/icon'
import {MatInputModule} from '@angular/material/input'
import {MatListModule} from '@angular/material/list'
import {MatSelectModule} from '@angular/material/select'
import {MatSidenavModule} from '@angular/material/sidenav'
import {MatSnackBarModule} from '@angular/material/snack-bar'
import {MatToolbarModule} from '@angular/material/toolbar'

@NgModule( {
  imports: [
    MatButtonModule, MatCardModule, MatCheckboxModule, MatDialogModule, MatDividerModule, MatFormFieldModule,
    MatIconModule, MatInputModule, MatListModule, MatRippleModule, MatSelectModule, MatSnackBarModule,
    MatSidenavModule, MatToolbarModule
  ],
  exports: [
    MatButtonModule, MatCardModule, MatCheckboxModule, MatDialogModule, MatDividerModule, MatFormFieldModule,
    MatIconModule, MatInputModule, MatListModule, MatRippleModule, MatSelectModule, MatSnackBarModule,
    MatSidenavModule, MatToolbarModule
  ]
} )
export class MaterialModule {
}
