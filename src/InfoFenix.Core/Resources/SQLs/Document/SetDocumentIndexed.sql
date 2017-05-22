UPDATE [documents] SET
    [indexed] = @Indexed,
    [payload] = @Payload
WHERE [document_id] = @DocumentID