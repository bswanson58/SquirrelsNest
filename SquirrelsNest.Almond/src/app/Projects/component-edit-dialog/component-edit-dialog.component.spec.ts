import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ComponentEditDialogComponent } from './component-edit-dialog.component';

describe('ComponentEditDialogComponent', () => {
  let component: ComponentEditDialogComponent;
  let fixture: ComponentFixture<ComponentEditDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ComponentEditDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ComponentEditDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
