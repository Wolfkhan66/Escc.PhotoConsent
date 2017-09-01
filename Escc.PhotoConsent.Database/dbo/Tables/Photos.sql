CREATE TABLE [dbo].[Photos] (
    [PhotoID]       INT   IDENTITY (1, 1) NOT NULL,
    [ParticipantID] INT   NOT NULL,
    [Image]         IMAGE NOT NULL,
    CONSTRAINT [PK_Photos] PRIMARY KEY CLUSTERED ([PhotoID] ASC),
    CONSTRAINT [FK_Photos_Participants] FOREIGN KEY ([ParticipantID]) REFERENCES [dbo].[Participants] ([ParticipantID])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Photos] TO [PhotoConsentUser]
    AS [dbo];

