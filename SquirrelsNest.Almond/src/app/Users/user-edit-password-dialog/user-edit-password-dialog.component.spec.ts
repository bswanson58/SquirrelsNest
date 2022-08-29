import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserEditPasswordDialogComponent } from './user-edit-password-dialog.component';

describe('UserEditPasswordDialogComponent', () => {
  let component: UserEditPasswordDialogComponent;
  let fixture: ComponentFixture<UserEditPasswordDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserEditPasswordDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserEditPasswordDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
