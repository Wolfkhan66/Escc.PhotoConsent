CREATE TABLE [dbo].[Participants] (
    [ParticipantID] INT          IDENTITY (1, 1) NOT NULL,
    [FormID]        INT          NOT NULL,
    [Name]          VARCHAR (50) NOT NULL,
    [Email]         VARCHAR (50) NULL,
    [ContactNumber] VARCHAR (50) NULL,
    CONSTRAINT [PK_Participants] PRIMARY KEY CLUSTERED ([ParticipantID] ASC),
    CONSTRAINT [FK_Participants_ConsentForms] FOREIGN KEY ([FormID]) REFERENCES [dbo].[ConsentForms] ([FormID])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Participants] TO [PhotoConsentUser]
    AS [dbo];

