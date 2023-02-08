import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IssueDetailSelectorComponent } from './issue-detail-selector.component';

describe('IssueDetailSelectorComponent', () => {
  let component: IssueDetailSelectorComponent;
  let fixture: ComponentFixture<IssueDetailSelectorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ IssueDetailSelectorComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IssueDetailSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
