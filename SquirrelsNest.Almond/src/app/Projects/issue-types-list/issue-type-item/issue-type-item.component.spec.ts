import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IssueTypeItemComponent } from './issue-type-item.component';

describe('IssueTypeItemComponent', () => {
  let component: IssueTypeItemComponent;
  let fixture: ComponentFixture<IssueTypeItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ IssueTypeItemComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IssueTypeItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
