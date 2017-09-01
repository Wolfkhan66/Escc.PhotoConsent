CREATE TABLE [dbo].[ConsentForms] (
    [FormID]        INT            IDENTITY (1, 1) NOT NULL,
    [DateCreated]   NVARCHAR (50)  NOT NULL,
    [CreatedBy]     NVARCHAR (50)  NOT NULL,
    [ProjectName]   NVARCHAR (50)  NULL,
    [DateSubmitted] NVARCHAR (50)  NULL,
    [ConsentGiven]  BIT            NOT NULL,
    [Notes]         NVARCHAR (MAX) NULL,
    [GUID]          NVARCHAR (MAX) NOT NULL,
    [Deleted]       BIT            NOT NULL,
    [PaymoNumber]   NVARCHAR (50)  NULL,
    CONSTRAINT [PK_ConsentForms] PRIMARY KEY CLUSTERED ([FormID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[ConsentForms] TO [PhotoConsentUser]
    AS [dbo];

