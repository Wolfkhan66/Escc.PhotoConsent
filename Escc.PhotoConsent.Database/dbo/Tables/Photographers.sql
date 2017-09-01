CREATE TABLE [dbo].[Photographers] (
    [PhotographerID] INT          IDENTITY (1, 1) NOT NULL,
    [FormID]         INT          NULL,
    [Name]           VARCHAR (50) NULL,
    [Email]          VARCHAR (50) NULL,
    [ContactNumber]  VARCHAR (50) NULL,
    CONSTRAINT [PK_Photographers] PRIMARY KEY CLUSTERED ([PhotographerID] ASC),
    CONSTRAINT [FK_Photographers_ConsentForms] FOREIGN KEY ([FormID]) REFERENCES [dbo].[ConsentForms] ([FormID])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Photographers] TO [PhotoConsentUser]
    AS [dbo];

