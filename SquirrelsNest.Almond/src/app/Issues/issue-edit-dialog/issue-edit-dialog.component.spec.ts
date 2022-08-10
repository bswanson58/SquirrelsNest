import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IssueEditDialogComponent } from './issue-edit-dialog.component';

describe('IssueEditDialogComponent', () => {
  let component: IssueEditDialogComponent;
  let fixture: ComponentFixture<IssueEditDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ IssueEditDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IssueEditDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
