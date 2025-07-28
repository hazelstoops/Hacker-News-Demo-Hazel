import { StoryService } from '../../service/story.service';
import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { NgxPaginationModule } from 'ngx-pagination';

import { StoryTableComponent } from './story-table.component';

import { of } from 'rxjs';
import { By } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

describe('StoryTableComponent', () => {
  let component: StoryTableComponent;
  let fixture: ComponentFixture<StoryTableComponent>;

  beforeEach(async () => {
    const storyServiceSpy = jasmine.createSpyObj<StoryService>(['getStories']);
    const mockData = {
      items: [
        {
          title: 'Title 1',
          text: 'This is some text',
          url: 'www.story1.com'
        },
        {
          title: 'Title 2',
          text: 'This is some text for story 2',
          url: 'www.story2.com'
        },
      ],
      pageSize: 10,
      pageNumber: 1,
      totalCount: 2
    };
    storyServiceSpy.getStories.and.callFake(function () {
      return of(mockData)
    });
    await TestBed.configureTestingModule({
      declarations: [StoryTableComponent],
      imports: [NgxPaginationModule, FormsModule],
      providers: [
        {
          provide: StoryService,
          useValue: storyServiceSpy
        }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StoryTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  afterEach(() => {
  });

  it('should create the app', fakeAsync(() => {
    component.ngOnInit();
    //tick(1000);
    tick();
    fixture.detectChanges();
    expect(component).toBeTruthy();
    //console.log(fixture.debugElement);
    console.log(fixture.debugElement.query(By.css('a')));
    let anchors = fixture.debugElement.queryAll(By.css('a'));
    console.log(anchors);
    //let anchors = fixture.debugElement.query(By.css('a'));
    //expect(anchors.
    //expect().toBeTruthy();
    expect(anchors[0].nativeNode.textContent).toEqual("Title 1");
    expect(anchors[1].nativeNode.textContent).toEqual("Title 2");
    
  }));

});
