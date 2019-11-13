//
//  ReadResultsViewController.h
//  URMA
//
//  Created by Raju on 24/09/14.
//  Copyright (c) 2014 Trimble Inc. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Global.h"
#import "HVTableView.h"
#import "tm_reader.h"
#import "ReadResultTableViewCell.h"


@interface ReadResultsViewController : UIViewController<HVTableViewDelegate, HVTableViewDataSource,UIActionSheetDelegate,ReadResultTableViewCellDelegate>{
    
}

@property (nonatomic, retain) UIActivityIndicatorView *spinner;

@property (nonatomic, retain) NSTimer *silenceTimer;
@property (weak, nonatomic) IBOutlet UIView *readResultsHeaderView;

@property (strong, nonatomic) IBOutlet HVTableView *readResultsTableView;
@property (weak, nonatomic) IBOutlet UILabel *readStatuslbl;

@property (weak, nonatomic) IBOutlet UIButton *refreshBtn;
@property (weak, nonatomic) IBOutlet UIButton *sortBtn;
@property (weak, nonatomic) IBOutlet UIButton *clearBtn;
@property (weak, nonatomic) IBOutlet UIButton *stopreadBtn;
@property (weak, nonatomic) IBOutlet UIButton *settingBtn;
@property (weak, nonatomic) IBOutlet UIButton *readBtn;


@property (weak, nonatomic) IBOutlet UILabel *uniqtagslblTxt;
@property (weak, nonatomic) IBOutlet UILabel *totaltagslblTxt;
@property (weak, nonatomic) IBOutlet UILabel *timesinseclblTxt;
@property (weak, nonatomic) IBOutlet UILabel *tagseclblTxt;

@property (weak, nonatomic) IBOutlet UILabel *uniqtagsValue;
@property (weak, nonatomic) IBOutlet UILabel *totaltagsValue;
@property (weak, nonatomic) IBOutlet UILabel *timeinsecValue;
@property (weak, nonatomic) IBOutlet UILabel *tagsecValue;


@property (weak, nonatomic) IBOutlet UIButton *firstBtn;

@property (weak, nonatomic) IBOutlet UIButton *previousBtn;
@property (weak, nonatomic) IBOutlet UIButton *nextBtn;
@property (weak, nonatomic) IBOutlet UIButton *lastBtn;

@property (weak, nonatomic) IBOutlet UILabel *curPage;
@property (weak, nonatomic) IBOutlet UILabel *totalPages;

@property (nonatomic, strong) NSData * TMR_Reader_data;


@end
