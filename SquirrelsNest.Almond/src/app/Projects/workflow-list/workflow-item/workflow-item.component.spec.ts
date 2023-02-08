import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowItemComponent } from './workflow-item.component';

describe('WorkflowItemComponent', () => {
  let component: WorkflowItemComponent;
  let fixture: ComponentFixture<WorkflowItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WorkflowItemComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WorkflowItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
