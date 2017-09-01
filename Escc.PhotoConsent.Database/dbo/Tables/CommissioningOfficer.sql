CREATE TABLE [dbo].[CommissioningOfficer] (
    [OfficerID]     INT          IDENTITY (1, 1) NOT NULL,
    [FormID]        INT          NOT NULL,
    [Name]          VARCHAR (50) NOT NULL,
    [Email]         VARCHAR (50) NULL,
    [ContactNumber] VARCHAR (50) NULL,
    CONSTRAINT [PK_CommissioningOfficer] PRIMARY KEY CLUSTERED ([OfficerID] ASC),
    CONSTRAINT [FK_CommissioningOfficer_ConsentForms] FOREIGN KEY ([FormID]) REFERENCES [dbo].[ConsentForms] ([FormID])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[CommissioningOfficer] TO [PhotoConsentUser]
    AS [dbo];

