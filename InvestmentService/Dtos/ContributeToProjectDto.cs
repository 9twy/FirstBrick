using System.ComponentModel.DataAnnotations;

public record class ContributeToProjectDto(
    [Required] int ProjectId,
    [Required] int Units
);