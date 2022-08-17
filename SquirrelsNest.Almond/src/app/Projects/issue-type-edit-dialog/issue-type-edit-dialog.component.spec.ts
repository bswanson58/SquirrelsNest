import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IssueTypeEditDialogComponent } from './issue-type-edit-dialog.component';

describe('IssueTypeEditDialogComponent', () => {
  let component: IssueTypeEditDialogComponent;
  let fixture: ComponentFixture<IssueTypeEditDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ IssueTypeEditDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IssueTypeEditDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
