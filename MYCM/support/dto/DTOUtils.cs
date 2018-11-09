using System.Collections.Generic;

namespace support.dto{
    public class DTOUtils{

        /// <summary>
        /// Reverses a DTO into his respective entity
        /// </summary>
        /// <param name="parseableDTO">DTOParseable with the DTO being reversed</param>
        /// <typeparam name="T">Generic-Type of the entity being reversed from the DTO</typeparam>
        /// <typeparam name="D">Generic-Type of the DTO being reversed</typeparam>
        /// <returns>T with the reversed DTO entity</returns>
        public static T reverseDTO<T,D>(DTOParseable<T,D> parseableDTO)where T:DTOAble<D> where D:DTO{
            return parseableDTO.toEntity();
        }

        /// <summary>
        /// Reverses an enumerable of DTO into their respective entities
        /// </summary>
        /// <param name="parseableDTOS">IEnumerable with the DTO being reversed</param>
        /// <typeparam name="T">Generic-Type of the entity being reversed from the DTO</typeparam>
        /// <typeparam name="D">Generic-Type of the DTO being reversed</typeparam>
        /// <returns>Enumerable with the reversed DTO entities</returns>
        public static IEnumerable<T> reverseDTOS<T,D>(IEnumerable<DTOParseable<T,D>> parseableDTOS)where T:DTOAble<D> where D:DTO{
            List<T> reversedDTOS=new List<T>();
            foreach(DTOParseable<T,D> parseableDTO in parseableDTOS)reversedDTOS.Add(parseableDTO.toEntity());
            return reversedDTOS;
        }

        /// <summary>
        /// Parses a DTOAble into a DTO
        /// </summary>
        /// <param name="dtoable">DTOAble with the entity which will parse to DTO</param>
        /// <typeparam name="D">Generic-Type of the DTO being created</typeparam>
        /// <returns>D with the parsed DTO from the DTOAble entity</returns>
        public static D parseToDTO<D>(DTOAble<D> dtoable) where D:DTO{
            return dtoable.toDTO();
        }

        /// <summary>
        /// Parses an enumerable of DTOAble into dtos
        /// </summary>
        /// <param name="dtoables">IEnumerable with the dtoables being parsed to dtos</param>
        /// <typeparam name="D">Generic-Type of the DTO being created</typeparam>
        /// <returns>IEnumerable with the parsed dtos</returns>
        public static IEnumerable<D> parseToDTOS<D>(IEnumerable<DTOAble<D>> dtoables)where D:DTO{
            List<D> dtos=new List<D>();
            foreach(DTOAble<D> dtoable in dtoables)dtos.Add(dtoable.toDTO());
            return dtos;
        }
    }
}