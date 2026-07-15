using System.Collections.Generic;
using App.Core.Models;

namespace App.Desktop.Features.Warp;

/// <summary>
/// The built-in warp locations seeded into <c>AppSettings.WarpLocations</c> when none are stored
/// (31 entries).
/// </summary>
public static class WarpDefaults
{
    public static List<WarpLocationSettings> Create() =>
    [
        new WarpLocationSettings { X = 152465, Y = 76951, Name = "Horizon" },
        new WarpLocationSettings { X = 151896, Y = 72599, Name = "Horizon Arena" },
        new WarpLocationSettings { X = 139369, Y = 73704, Name = "Witch" },
        new WarpLocationSettings { X = 132995, Y = 87096, Name = "Moonlight 1" },
        new WarpLocationSettings { X = 130842, Y = 79586, Name = "Moonlight 2" },
        new WarpLocationSettings { X = 155817, Y = 103724, Name = "Lost Mines 1" },
        new WarpLocationSettings { X = 152309, Y = 102886, Name = "Lost Mines 2" },
        new WarpLocationSettings { X = 117974, Y = 59119, Name = "Katan" },
        new WarpLocationSettings { X = 7363, Y = 7101, Name = "Laksy" },
        new WarpLocationSettings { X = 135876, Y = 156035, Name = "Fairy's Woods" },
        new WarpLocationSettings { X = 142086, Y = 132207, Name = "Siren Island" },
        new WarpLocationSettings { X = 138174, Y = 105965, Name = "Rondo" },
        new WarpLocationSettings { X = 159712, Y = 124736, Name = "Beginning of Marduka" },
        new WarpLocationSettings { X = 162624, Y = 131811, Name = "Mare Village" },
        new WarpLocationSettings { X = 211302, Y = 129971, Name = "Palmir Plateau 1" },
        new WarpLocationSettings { X = 211299, Y = 146193, Name = "Palmir Plateau 2" },
        new WarpLocationSettings { X = 103210, Y = 100366, Name = "CV1" },
        new WarpLocationSettings { X = 99757, Y = 103236, Name = "CV2" },
        new WarpLocationSettings { X = 154666, Y = 150287, Name = "City of Ruins" },
        new WarpLocationSettings { X = 83876, Y = 115913, Name = "Veiled Island" },
        new WarpLocationSettings { X = 201400, Y = 151566, Name = "Temple of Ancient" },
        new WarpLocationSettings { X = 201400, Y = 167649, Name = "Temple of Lost Souls" },
        new WarpLocationSettings { X = 201400, Y = 135438, Name = "Temple of Exile" },
        new WarpLocationSettings { X = 98965, Y = 129209, Name = "The labyrinth" },
        new WarpLocationSettings { X = 38300, Y = 118304, Name = "Circus" },
        new WarpLocationSettings { X = 108660, Y = 76471, Name = "Remain of the Ancient" },
        new WarpLocationSettings { X = 98536, Y = 127399, Name = "Devildom" },
        new WarpLocationSettings { X = 20377, Y = 100948, Name = "Sky fortress" },
        new WarpLocationSettings { X = 222235, Y = 20078, Name = "Hidden village" },
        new WarpLocationSettings { X = 68953, Y = 23575, Name = "Twilight Underworld" },
        new WarpLocationSettings { X = 36342, Y = 104892, Name = "Al Catraz" },
    ];
}
