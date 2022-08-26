import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserEditRolesDialogComponent } from './user-edit-roles-dialog.component';

describe('UserEditRolesDialogComponent', () => {
  let component: UserEditRolesDialogComponent;
  let fixture: ComponentFixture<UserEditRolesDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserEditRolesDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserEditRolesDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
