import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IssueFooterComponent } from './issue-footer.component';

describe('IssueFooterComponent', () => {
  let component: IssueFooterComponent;
  let fixture: ComponentFixture<IssueFooterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ IssueFooterComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IssueFooterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
