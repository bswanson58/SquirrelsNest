import {Directive, Input, TemplateRef, ViewContainerRef} from '@angular/core'

// from: https://stackoverflow.com/questions/38582293/how-to-declare-a-variable-in-a-template-in-angular
// usage: <ng-container *ngVar='componentVariable as variable'></ng-container>
//        <ng-container *ngVar='componentVariable$ | async as someName'>
//          <div *ngIf='someName'></div>
//        </ng-container>
@Directive( {
  selector: '[ngVar]',
} )
export class VariableDirective {
  @Input()
  set ngVar( context: unknown ) {
    this.context.$implicit = this.context.ngVar = context

    if( !this.hasView ) {
      this.vcRef.createEmbeddedView( this.templateRef, this.context )
      this.hasView = true
    }
  }

  private context: {
    $implicit: unknown;
    ngVar: unknown;
  } = {
    $implicit: null,
    ngVar: null,
  }

  private hasView: boolean = false

  constructor( private templateRef: TemplateRef<any>, private vcRef: ViewContainerRef ) {
  }
}
