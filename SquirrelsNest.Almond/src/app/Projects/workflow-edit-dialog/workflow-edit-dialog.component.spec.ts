import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowEditDialogComponent } from './workflow-edit-dialog.component';

describe('WorkflowEditDialogComponent', () => {
  let component: WorkflowEditDialogComponent;
  let fixture: ComponentFixture<WorkflowEditDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WorkflowEditDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WorkflowEditDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
